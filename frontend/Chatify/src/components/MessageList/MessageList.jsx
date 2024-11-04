import { PlusOutlined } from '@ant-design/icons'
import React from 'react'
import MessageItem from './MessageItem'

function MessageList({ navigate, messageList }) {
  const sortedMessageList = [...messageList].sort((a, b) => {
    return new Date(b.id.createdAt) - new Date(a.id.createdAt);
  });

  return (
    <div className="mb-2 flex h-1/3 w-full flex-col overflow-hidden rounded-t-xl border-2 border-gray-500 md:mb-0 md:h-full md:w-96">
      <div className="flex w-full items-center p-2 font-bold text-white md:p-3">
        <div className="w-3/4 text-xl leading-7">Chats</div>
        <div className="flex flex-grow items-center justify-center">
          <button
            className="flex flex-grow items-center justify-center"
            aria-label="New thread"
            title="New Thread"
            onClick={() => navigate("/home/newChat")}
          >
            <div className="flex h-8 w-8 items-center justify-center rounded-full border-2 border-gray-700 bg-gray-600">
              <PlusOutlined size={16} />
            </div>
          </button>
        </div>
      </div>
      <div id="messageList" className="flex-grow overflow-y-auto">
        {/* Message items go here */}
        {sortedMessageList.map((message) => (
          <MessageItem
            key={message.id.id}
            sender={message.id.creatorId}
            time={new Date(message.id.createdAt).toLocaleString()}
            latestMessage={message.id.title} 
            onClick={() => navigate("/home/chat")}/>
        ))}
      </div>
    </div>
  )
}

export default MessageList