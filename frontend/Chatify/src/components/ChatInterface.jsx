import React, { useEffect, useRef, useState } from 'react'
import ChatMessage from './ChatMessage';
import useSignalR from '../config/signalr';
import { message } from 'antd';
import * as libsignal from '@privacyresearch/libsignal-protocol-typescript';
import {
    store,
    base64ToArrayBuffer,
    arrayBufferToBase64,
} from '../services/encryptionService';
import api from '../config/axios';

async function establishSessionWithUser(senderIdStr) {
    // Fetch the sender's public keys from the server
    const response = await api.get(`keys/${senderIdStr}`);
    const preKeyBundle = response.data;
  
    // Convert base64 keys to SignalPublicKey instances
    const processedPreKeyBundle = {
      identityKey: await libsignal.SignalPublicKey.deserialize(
        base64ToArrayBuffer(preKeyBundle.identityKey)
      ),
      registrationId: preKeyBundle.registrationId,
      preKey: {
        keyId: preKeyBundle.preKey.keyId,
        publicKey: await libsignal.SignalPublicKey.deserialize(
          base64ToArrayBuffer(preKeyBundle.preKey.publicKey)
        ),
      },
      signedPreKey: {
        keyId: preKeyBundle.signedPreKey.keyId,
        publicKey: await libsignal.SignalPublicKey.deserialize(
          base64ToArrayBuffer(preKeyBundle.signedPreKey.publicKey)
        ),
        signature: base64ToArrayBuffer(preKeyBundle.signedPreKey.signature),
      },
    };
  
    const address = new libsignal.SignalProtocolAddress(senderIdStr, 1);
    const sessionBuilder = new libsignal.SessionBuilder(store, address);
  
    await sessionBuilder.processPreKey(processedPreKeyBundle);
  }


const ChatInterface = () => {
    const [messages, setMessages] = useState([]);
    const [newMessage, setNewMessage] = useState('');
    const [userId, setUserId] = useState('');
    const messagesEndRef = useRef(null);

    // Initialize SignalR connection
    const { connection, error, invokeWithAuth } = useSignalR();

    const scrollToBottom = () => {
        messagesEndRef.current?.scrollIntoView({ behavior: "smooth" });
    };

    useEffect(() => {
        // Get user ID from authentication context or local storage
        const currentUserId = sessionStorage.getItem('userId') || '';
        setUserId(currentUserId);

        // Set up SignalR message handler
        if (connection) {
            connection.on("ReceiveMessage", async (senderId, encryptedMessage) => {
                const senderIdStr = String(senderId);
                const isUser = senderIdStr === String(userId);
                const senderName = isUser ? "You" : `User ${senderIdStr.substr(0, 4)}`;

                const address = new libsignal.SignalProtocolAddress(senderIdStr, 1);
                const sessionCipher = new libsignal.SessionCipher(store, address);

                let plaintextMessage;

                try {
                    const messageType = encryptedMessage.type;
                    const messageBody = base64ToArrayBuffer(encryptedMessage.body);

                    let decryptedArrayBuffer;
                    if (messageType === 3) {
                        decryptedArrayBuffer = await sessionCipher.decryptPreKeyWhisperMessage(
                            messageBody,
                            'binary'
                        );
                    } else if (messageType === 1) {
                        decryptedArrayBuffer = await sessionCipher.decryptWhisperMessage(
                            messageBody,
                            'binary'
                        );
                    } else {
                        throw new Error('Unknown message type');
                    }

                    plaintextMessage = new TextDecoder().decode(decryptedArrayBuffer);
                } catch (e) {
                    console.error('Decryption failed:', e);

                    // If no session exists, establish one
                    await establishSessionWithUser(senderIdStr);

                    // Retry decryption
                    const messageType = encryptedMessage.type;
                    const messageBody = base64ToArrayBuffer(encryptedMessage.body);

                    let decryptedArrayBuffer;
                    if (messageType === 3) {
                        decryptedArrayBuffer = await sessionCipher.decryptPreKeyWhisperMessage(
                            messageBody,
                            'binary'
                        );
                    } else if (messageType === 1) {
                        decryptedArrayBuffer = await sessionCipher.decryptWhisperMessage(
                            messageBody,
                            'binary'
                        );
                    } else {
                        throw new Error('Unknown message type');
                    }

                    plaintextMessage = new TextDecoder().decode(decryptedArrayBuffer);
                }

                setMessages((prev) => [
                    ...prev,
                    {
                        sender: senderName,
                        message: plaintextMessage,
                        isUser: isUser,
                        timestamp: new Date(),
                    },
                ]);
            });
        }

        return () => {
            if (connection) {
                connection.off("ReceiveMessage");
            }
        };
    }, [connection]);

    useEffect(() => {
        scrollToBottom();
    }, [messages]);

    const handleSendMessage = async (e) => {
        e.preventDefault();
    
        if (!newMessage.trim() || !connection) return;
    
        try {
            // Fetch list of user IDs from the server
            const res = await api.get("/users");
            const userIds = res.data;
    
            // Encrypt the message for each user
            for (const recipientId of userIds) {
                if (recipientId === userId) continue; // Skip self
    
                const recipientIdStr = String(recipientId);
                const address = new libsignal.SignalProtocolAddress(recipientIdStr, 1);
                const sessionCipher = new libsignal.SessionCipher(store, address);
    
                // Ensure plaintext is an ArrayBuffer
                const plaintextArrayBuffer = new TextEncoder().encode(newMessage).buffer;
    
                // Try to encrypt
                let ciphertext;
                try {
                    ciphertext = await sessionCipher.encrypt(plaintextArrayBuffer);
                } catch (e) {
                    console.error('Encryption failed:', e);
    
                    // No session exists, establish one
                    await establishSessionWithUser(recipientIdStr);
    
                    // Retry encryption
                    ciphertext = await sessionCipher.encrypt(plaintextArrayBuffer);
                }
    
                // Send the encrypted message to the recipient via the server
                await invokeWithAuth('SendEncryptedMessage', recipientIdStr, {
                    type: ciphertext.type,
                    body: arrayBufferToBase64(ciphertext.body),
                });
            }
    
            setNewMessage('');
        } catch (err) {
            message.error('Error sending message:', err.message);
            console.error('Error sending message:', err);
        }
    };

    return (
        <div className="flex flex-col flex-auto flex-shrink-0 bg-gray-900 h-full p-4">
            <div className="flex flex-col h-full overflow-x-auto mb-4">
                <div className="flex flex-col h-full">
                    <div className="grid grid-cols-12 gap-y-2">
                        {messages.map((msg, index) => (
                            <ChatMessage
                                key={index}
                                message={msg.message}
                                sender={msg.sender}
                                isUser={msg.isUser}
                            />
                        ))}
                        <div ref={messagesEndRef} />
                    </div>
                </div>
            </div>

            <form onSubmit={handleSendMessage} className="flex flex-row items-center h-16 rounded-xl bg-white w-full px-4">
                <div>
                    <button type="button" className="flex items-center justify-center text-gray-400 hover:text-gray-600">
                        <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M15.172 7l-6.586 6.586a2 2 0 102.828 2.828l6.414-6.586a4 4 0 00-5.656-5.656l-6.415 6.585a6 6 0 108.486 8.486L20.5 13"></path>
                        </svg>
                    </button>
                </div>
                <div className="flex-grow ml-4">
                    <div className="relative w-full">
                        <input
                            type="text"
                            value={newMessage}
                            onChange={(e) => setNewMessage(e.target.value)}
                            className="flex w-full border rounded-xl focus:outline-none focus:border-indigo-300 pl-4 h-10"
                            placeholder="Type your message..."
                        />
                    </div>
                </div>
                <div className="ml-4">
                    <button
                        type="submit"
                        className="flex items-center justify-center bg-indigo-500 hover:bg-indigo-600 rounded-xl text-white px-4 py-1 flex-shrink-0"
                    >
                        <span>Send</span>
                        <span className="ml-2">
                            <svg className="w-4 h-4 transform rotate-45 -mt-px" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M12 19l9 2-9-18-9 18 9-2zm0 0v-8"></path>
                            </svg>
                        </span>
                    </button>
                </div>
            </form>
        </div>
    );
};

export default ChatInterface;