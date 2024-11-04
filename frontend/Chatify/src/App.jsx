import { RouterProvider } from "react-router-dom";
import { router } from "./config/router";
import "./index.css";

function App() {
  return (
      <div className="bg-gray-900 font-mono antialiased h-screen">
        <RouterProvider router={router} />
      </div>
  );
}

export default App;
