import React from 'react'

function ChatMessage(props) {
  const { message, sender, isUser } = props;
  const senderName = String(sender);
  return (
    <div className={`p-3 rounded-lg ${isUser ? 'col-start-1 col-end-8' : 'col-start-6 col-end-13'}`}>
      <div className={`flex items-center ${isUser ? 'flex-row' : 'justify-start flex-row-reverse'}`}>
        <div className="flex items-center justify-center h-10 w-10 rounded-full bg-indigo-500 flex-shrink-0 text-white">
          {senderName.charAt(0)}
        </div>
        <div className={`relative ${isUser ? 'ml-3' : 'mr-3'} flex flex-col`}>
          <span className={`text-xs text-gray-500 mb-1 ${isUser ? 'text-left' : 'text-right'}`}>
            {senderName}
          </span>
          <div className={`text-sm py-2 px-4 shadow rounded-xl ${isUser ? 'bg-white' : 'bg-indigo-100'
            }`}>
            {message}
          </div>
        </div>
      </div>
    </div>
  );
}

export default ChatMessage