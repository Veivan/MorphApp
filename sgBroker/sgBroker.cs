using System;

using ZeroMQ;

namespace sgBroker
{
    class sgBroker
    {
        public void Run()
        {
            //using (var ctx = new ZContext()) 
            using (var frontend = new ZSocket(ZSocketType.ROUTER))
            using (var backend = new ZSocket(ZSocketType.DEALER))
            {
                frontend.Bind("tcp://*:5559");
                backend.Bind("tcp://*:5560");

                // Initialize poll set
                var poll = ZPollItem.CreateReceiver();

                // Switch messages between sockets
                ZError error;
                ZMessage message;
                while (true)
                {
                    if (frontend.PollIn(poll, out message, out error, TimeSpan.FromMilliseconds(64)))
                    {
                        // Process all parts of the message
                        //Console_WriteZMessage("frontend", 2, message);
                        backend.Send(message);
                    }
                    else
                    {
                        if (error == ZError.ETERM)
                            return;    // Interrupted
                        if (error != ZError.EAGAIN)
                            throw new ZException(error);
                    }

                    if (backend.PollIn(poll, out message, out error, TimeSpan.FromMilliseconds(64)))
                    {
                        // Process all parts of the message
                        //Console_WriteZMessage(" backend", 2, message);
                        frontend.Send(message);
                    }
                    else
                    {
                        if (error == ZError.ETERM)
                            return;    // Interrupted
                        if (error != ZError.EAGAIN)
                            throw new ZException(error);
                    }
                }
            }
        }
    }
}
