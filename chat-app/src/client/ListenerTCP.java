package client;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import java.net.Socket;

public class ListenerTCP extends Thread{

    private final MessagesQueue messagesReceived;

    private PrintWriter out;
    private BufferedReader in;

    public ListenerTCP(MessagesQueue messagesReceivedQueue, Socket socket){
        messagesReceived = messagesReceivedQueue;
        try {
            out = new PrintWriter(socket.getOutputStream(), true);
            in = new BufferedReader(new InputStreamReader(socket.getInputStream()));
        } catch (IOException e){
            e.printStackTrace();
        }
    }

    @Override
    public void run(){
        do {
            String message = null;
            try {
                message = in.readLine();
            } catch (IOException e) {
                System.out.println("Finished successfully");
            }
            if (message != null && message.length() > 0) {
                messagesReceived.push(message);
            }
        } while (!isInterrupted());

    }

    public PrintWriter getOut() {
        return out;
    }
}
