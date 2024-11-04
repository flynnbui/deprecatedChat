import React from 'react'

function NavButton ({ icon, label, isActive, onClick }) {
    return (
      <div className="h-12 text-center text-white md:h-16">
        <button
          className={`h-12 w-16 text-center text-white md:h-16 
            ${isActive ? "bg-sky-900" : "md:hover:bg-sky-900"}`}
          aria-label={label}
          title={label}
          onClick={onClick}
        >
          {icon}
        </button>
      </div>
    );
  };

export default NavButton