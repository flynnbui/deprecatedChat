import React, { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import api from "../../config/axios.js";
import { message } from "antd";

function RegisterPage() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!username || !password) {
      return message.error("Username and password are required.");
    }
    if(confirmPassword != password) {
      return message.error("Password does not match!");
    }

    const payload = {
      userName: username,
      password: password,
    };

    try {
      const response = await api.post("/auth/register", payload);

      if (response.status === 200) {
        message.success("Register Successful!");
        navigate("/login");
      } else {
        throw new Error(`Unexpected response status: ${response.status}`);
      }
    } catch (error) {
      console.error("Registration error:", error);
      let errorMessage;
      if (error.response) {
        if (error.response.data && Array.isArray(error.response.data.errors)) {
          errorMessage = error.response.data.errors.join(" ");
        } else {
          errorMessage = error.response.data.message || "Unknown error occurred";
        }
      } else {
        errorMessage = error.message || "Network error occurred";
      }
      message.error(`Register Failed: ${errorMessage}`);
    }
  };

  return (
    <div id="registerPage">
      <div className="flex h-screen flex-col justify-center px-6 py-12 lg:px-8">
        <div className="sm:mx-auto sm:w-full sm:max-w-sm">
          <div className="text-white">Chatify</div>
          <h2 className="mt-10 text-center text-2xl font-bold leading-9 tracking-tight text-white">
            Register your account
          </h2>
        </div>

        <div className="mt-10 sm:mx-auto sm:w-full sm:max-w-sm">
          <form className="space-y-6" id="registerForm" onSubmit={handleSubmit}>
            {/* Username */}
            <div>
              <label
                htmlFor="username"
                className="block text-sm font-medium leading-6 text-gray-300"
              >
                Username
              </label>
              <input
                id="username"
                name="username"
                type="text"
                required
                className="block w-full rounded-md border-0 bg-gray-700 text-white placeholder:text-gray-400 shadow-sm ring-1 ring-inset ring-gray-600 py-1.5 focus:ring-2 focus:ring-inset focus:ring-indigo-500 sm:text-sm sm:leading-6"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
              />
            </div>
            {/* Password */}
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
                  required
                  className="block w-full rounded-md border-0 bg-gray-700 text-white placeholder:text-gray-400 shadow-sm ring-1 ring-inset ring-gray-600 py-1.5 focus:ring-2 focus:ring-inset focus:ring-indigo-500 sm:text-sm sm:leading-6"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                />
              </div>
            </div>
            {/* Confirm password */}
            <div>
              <div className="flex items-center justify-between">
                <label
                  htmlFor="confirmPassword"
                  className="block text-sm font-medium leading-6 text-gray-300"
                >
                  Confirm your Password
                </label>
              </div>
              <div className="mt-2">
                <input
                  id="confirmPassword"
                  name="confirmPassword"
                  type="password"
                  required
                  value={confirmPassword}
                  onChange={(e) => setConfirmPassword(e.target.value)}
                  className="block w-full rounded-md border-0 bg-gray-700 text-white placeholder:text-gray-400 shadow-sm ring-1 ring-inset ring-gray-600 py-1.5 focus:ring-2 focus:ring-inset focus:ring-indigo-500 sm:text-sm sm:leading-6"
                />
              </div>
            </div>

            <div>
              <button
                id="registerButton"
                type="submit"
                className="flex w-full justify-center rounded-md bg-indigo-600 px-3 text-sm font-semibold leading-6 text-white shadow-sm py-1.5 hover:bg-indigo-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600"
              >
                Register
              </button>
            </div>
          </form>

          <p className="mt-10 text-center text-sm text-gray-400">
            Already have an account?
            <Link
              to="/login"
              className="font-semibold leading-6 text-indigo-400 hover:text-indigo-300 ml-2"
            >
              Login here
            </Link>
          </p>
        </div>
      </div >
    </div >
  );
}

export default RegisterPage;
