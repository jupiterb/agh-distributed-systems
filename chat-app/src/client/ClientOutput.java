package client;

public class ClientOutput extends Thread {

    private final MessagesQueue messagesReceived;

    public ClientOutput(MessagesQueue messagesReceivedQueue) {
        this.messagesReceived = messagesReceivedQueue;
    }

    @Override
    public void run(){
        while (true){
            if (messagesReceived.hasAny()){
                System.out.println(messagesReceived.pop());
            } else if (isInterrupted()){
                break;
            }
        }
    }
}
