import { useState, useEffect, useCallback, } from 'react';
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { getJwtToken, getRefreshToken } from '../pages/auth/auth';

const baseURL = "http://localhost:5000/api/chat"
const useSignalR = () => {
    const [connection, setConnection] = useState(null);
    const [error, setError] = useState(null);

    // Function to refresh the token
    const refreshToken = async () => {
        try {
            const storedRefreshToken = getRefreshToken();
            const storedToken = getJwtToken();

            if (!storedRefreshToken) return null;

            const response = await fetch('/auth/refreshtoken', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    Token: storedToken,
                    RefreshToken: storedRefreshToken
                })
            });

            if (!response.ok) throw new Error('Failed to refresh token');

            const data = await response.json();
            localStorage.setItem('token', data.token);
            localStorage.setItem('refreshToken', data.refreshToken);

            return data.token;
        } catch (error) {
            console.error('Token refresh failed:', error);
            return null;
        }
    };

    // Function to create a new connection
    const createConnection = useCallback(() => {
        const newConnection = new HubConnectionBuilder()
            .withUrl(baseURL, {
                accessTokenFactory: async () => {
                    let token = getJwtToken();
                    if (!token) {
                        token = await refreshToken();
                    }
                    return token;
                }
            })
            .withAutomaticReconnect({
                nextRetryDelayInMilliseconds: retryContext => {
                    // Custom retry logic
                    if (retryContext.previousRetryCount === 0) {
                        // On first retry, attempt to refresh the token
                        return 0;
                    }
                    return Math.min(1000 * Math.pow(2, retryContext.previousRetryCount), 30000);
                }
            })
            .configureLogging(LogLevel.Information)
            .build();

        // Handle authentication errors
        newConnection.onclose(async (error) => {
            if (error) {
                console.error('Connection closed with error:', error);
                if (error.message === 'Unauthorized' || error.statusCode === 401) {
                    const newToken = await refreshToken();
                    if (newToken) {
                        // Attempt to reconnect with new token
                        await startConnection(newConnection);
                    }
                }
            }
        });

        return newConnection;
    }, [baseURL]);

    // Function to start the connection with error handling
    const startConnection = async (conn) => {
        try {
            await conn.start();
            console.log('SignalR Connected');
        } catch (err) {
            console.error('SignalR Connection Error:', err);
            if (err.statusCode === 401) {
                const newToken = await refreshToken();
                if (newToken) {
                    // Retry connection with new token
                    await conn.start();
                } else {
                    setError(new Error('Authentication failed'));
                }
            } else {
                setError(err);
            }
        }
    };

    useEffect(() => {
        const initializeConnection = async () => {
            const newConnection = createConnection();
            setConnection(newConnection);
            await startConnection(newConnection);
        };

        initializeConnection();

        return () => {
            if (connection) {
                connection.stop();
            }
        };
    }, [createConnection]);

    // Invoke hub methods with token refresh
    const invokeWithAuth = async (methodName, ...args) => {
        if (!connection) throw new Error('No connection available');

        try {
            return await connection.invoke(methodName, ...args);
        } catch (error) {
            if (error.statusCode === 401) {
                const newToken = await refreshToken();
                if (newToken) {
                    // Retry the method call with new token
                    return await connection.invoke(methodName, ...args);
                }
                throw new Error('Authentication failed');
            }
            throw error;
        }
    };

    return {
        connection,
        error,
        invokeWithAuth
    };
};


export default useSignalR;
