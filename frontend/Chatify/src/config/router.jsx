import { createBrowserRouter } from "react-router-dom";
import LoginPage from "../pages/auth/LoginPage.jsx";
import RegisterPage from "../pages/auth/RegisterPage.jsx";
import ChatInterface from "../components/ChatInterface";
import HomePage from "../pages/home/HomePage.jsx";

export const router = createBrowserRouter([
  {
    path: "/login",
    element : <LoginPage/>,
  },
  {
    path: "/register",
    element : <RegisterPage/>,
  },
  {
    path: "/home",
    element: <HomePage/>,
    children: [
      {
        path: "/home/chat",
        element: <ChatInterface/>
      }
    ]
  }
]);

