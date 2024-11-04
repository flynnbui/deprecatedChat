import React from 'react';

const MessageItem = ({ sender, time, latestMessage, onClick }) => {
  return (
    <>
      <button
        className="block h-24 w-full bg-gray-800 p-4 text-left threadItem hover:bg-gray-700"
        onClick={onClick} >
        <div className="flex h-full flex-col justify-between overflow-hidden">
          <div className="overflow-hidden">
            <h3 className="truncate text-sm font-semibold text-pink-200">{sender}</h3>
            <p className="truncate text-xs text-gray-200">{time}</p>
          </div>
          <span className="truncate text-xs text-gray-400">{latestMessage}</span>
        </div>
      </button>
      {/* Vertical Line Separator */}
      <div className="h-px w-full bg-gray-500"></div>
    </>
  );
};

export default MessageItem;