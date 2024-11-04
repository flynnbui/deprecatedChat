import { Link, useNavigate } from 'react-router-dom';
import React, { useState } from "react";
import api from '../../config/axios';
import { message } from 'antd';
import { initializeEncryption, setJwtToken, setRefreshToken, setUserDetail } from './auth';

function LoginPage() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();
  const handleSubmit = async (e) => {
    e.preventDefault();

    const payload = {
      userName: username,
      password: password
    };
    try {
      const response = await api.post("/auth/login", payload);
      if (response.status === 200) {
        setJwtToken(response.data.token);
        setRefreshToken(response.data.refreshToken);
        setUserDetail(response.data.token);
        message.success("Login Succesfully!");
        await initializeEncryption();
        navigate("/home");
      }
    }
    catch (error) {
      console.error(error);
      const errorMessage = error.response?.data?.error || 'An error occurred';
      message.error(`Login Failed: ${errorMessage}`);
    }
  };
  return (
    <div id="loginPage">
      <div className="flex h-screen flex-col justify-center px-25 py-12 lg:px-8">
        <div className="sm:mx-auto sm:w-full sm:max-w-sm">
          <div className="text-white">Chatify</div>
          <h2 className="mt-10 text-center text-2xl font-bold leading-9 tracking-tight text-white">
            Sign in to your account
          </h2>
        </div>

        <div className="mt-10 sm:mx-auto sm:w-full sm:max-w-sm">
          <form className="space-y-6" id="loginForm" onSubmit={handleSubmit}>
            <div>
              <label
                htmlFor="username"
                className="block text-sm font-medium leading-6 text-gray-300"
              >
                Username
              </label>
              <div className="mt-2">
                <input
                  id="username"
                  name="username"
                  type="text"
                  required
                  autoComplete="username"
                  className="block w-full rounded-md border-0 bg-gray-700 text-white placeholder:text-gray-400 shadow-sm ring-1 ring-inset ring-gray-600 py-1.5 focus:ring-2 focus:ring-inset focus:ring-indigo-500 sm:text-sm sm:leading-6"
                  value={username}
                  onChange={(e) => setUsername(e.target.value)}
                />
              </div>
            </div>

            <div>
              <div className="flex items-center justify-between">
                <label
                  htmlFor="password"
                  className="block text-sm font-medium leading-6 text-gray-300"
                >
                  Password
                </label>
              </div>
              <div className="mt-2">
                <input
                  id="password"
                  name="password"
                  type="password"
                  autoComplete="current-password"
                  required
                  className="block w-full rounded-md border-0 bg-gray-700 text-white placeholder:text-gray-400 shadow-sm ring-1 ring-inset ring-gray-600 py-1.5 focus:ring-2 focus:ring-inset focus:ring-indigo-500 sm:text-sm sm:leading-6"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                />
              </div>
            </div>

            <div>
              <button
                id="loginButton"
                type="submit"
                className="flex w-full justify-center rounded-md bg-indigo-600 px-3 text-sm font-semibold leading-6 text-white shadow-sm py-1.5 hover:bg-indigo-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600"
              >
                Sign in
              </button>
            </div>
          </form>

          <p className="mt-10 text-center text-sm text-gray-400">
            Not a member?
            <Link to="/register"
              className="font-semibold leading-6 text-indigo-400 hover:text-indigo-300 ml-2"
            >
              Register
            </Link>
          </p>
        </div>
      </div>
    </div>
  );
}

export default LoginPage;
