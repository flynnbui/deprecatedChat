import React, { useState } from "react";
import { Outlet, useNavigate } from "react-router-dom";
import Navbar from "../../components/Navigation/Navbar";
import Header from "../../components/Header";
import MessageList from "../../components/MessageList/MessageList";

const HomePage = () => {
  const [headerTitle, setHeaderTitle] = useState("Home");
  const navigate = useNavigate();

  const messageList = [
    {
      id: {
        id: 1,
        title: "",
        content: "",
        creatorId: "Global Chat",
        createdAt: new Date().toISOString(),
      },
    },
  ];

  return (
    <div className="flex h-screen w-full flex-col md:flex-row">
      {/* Navigation bar */}
      <Navbar setHeaderTitle={setHeaderTitle} />

      {/* Main */}
      <div className="flex flex-grow flex-col h-[calc(100vh-3rem)] md:h-screen">
        {/* Web's Header */}
        <Header title={headerTitle} />

        {/* Content Container */}
        <div className="flex flex-grow flex-col overflow-hidden px-2 md:flex-row md:px-0">
          {/* Threads List */}
          <MessageList navigate={navigate} messageList={messageList} />

          {/* Content */}
          <div className="flex h-2/3 w-full flex-grow flex-col items-center md:h-full">
            <div className="flex h-full w-full flex-col overflow-hidden rounded-t-xl border-2 border-gray-500 md:-ml-6 md:w-11/12">
              <Outlet />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default HomePage;
