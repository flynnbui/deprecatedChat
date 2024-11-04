import React from "react";

function Header({ title }) {
  return (
    <div className="my-2 hidden text-center text-2xl font-bold leading-7 text-white md:my-5 md:block">
      {title}
    </div>
  );
}

export default Header;
