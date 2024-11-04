import React, { useState } from "react";
import NavButton from "./NavButton";
import {
  BellOutlined,
  HomeOutlined,
  LogoutOutlined,
  PlusOutlined,
  UserOutlined,
} from "@ant-design/icons";

function Navbar({ setHeaderTitle }) {
  const [activeTab, setActiveTab] = useState("home");
  return (
    <nav className="flex h-12 w-full shrink-0 flex-row bg-inherit md:h-screen md:w-16 md:flex-col">
      {/* Logo */}
      <div className="hidden items-center justify-center text-center md:flex md:h-1/6">
        <div className="text-white">
          <button className="home">Chatify</button>
        </div>
      </div>

      {/* Navigation buttons */}
      <div className="flex h-full w-full flex-row justify-around md:h-4/6 md:w-16 md:flex-col md:justify-start">
        <NavButton
          icon={<HomeOutlined />}
          label="Home"
          isActive={activeTab === "home"}
          onClick={() => {
            setActiveTab("home");
            setHeaderTitle("Home");
          }}
        />
        <NavButton
          icon={<BellOutlined />}
          label="Notifications"
          isActive={activeTab === "notifications"}
          onClick={() => {
            setActiveTab("notifications");
            setHeaderTitle("Notifications");
          }}
        />
        <NavButton
          icon={<PlusOutlined />}
          label="New message"
          isActive={activeTab === "newMessage"}
          onClick={() => {
            setActiveTab("newMessage");
            setHeaderTitle("New Message");
          }}
        />
        <NavButton
          icon={<UserOutlined />}
          label="Profile"
          isActive={activeTab === "profile"}
          onClick={() => {
            setActiveTab("profile");
            setHeaderTitle("Profile");
          }}
        />
        <NavButton
          icon={<LogoutOutlined />}
          label="Logout"
          isActive={activeTab === "logout"}
          onClick={() => {
            setActiveTab("logout");
          }}
        />
      </div>
    </nav>
  );
}

export default Navbar;
