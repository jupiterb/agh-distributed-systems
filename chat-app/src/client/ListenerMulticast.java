package client;

import java.io.IOException;
import java.net.DatagramPacket;
import java.net.DatagramSocket;

public class ListenerMulticast extends Thread{

    private final MessagesQueue messagesReceived;

    private final DatagramSocket datagramSocket;

    public ListenerMulticast(MessagesQueue messagesReceivedQueue, DatagramSocket datagramSocket){
        messagesReceived = messagesReceivedQueue;
        this.datagramSocket = datagramSocket;
    }

    @Override
    public void run(){

        byte[] buff = new byte[1024];

        do {
            try {
                DatagramPacket datagramPacket = new DatagramPacket(buff, buff.length);
                datagramSocket.receive(datagramPacket);
                String message = new String(buff);
                if (datagramPacket.getPort() != datagramSocket.getPort()){
                    messagesReceived.push(message);
                }
            } catch (IOException e) {
                System.out.println("Finished successfully");
            }
        } while (!isInterrupted());
    }
}
