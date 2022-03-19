package server;

import java.io.IOException;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.SocketException;

public class ListenerServerUDP extends Thread {

    private final ServerContainer serverContainer;
    private final int serverPort;

    public ListenerServerUDP(ServerContainer serverContainer, int serverPort){
        this.serverContainer = serverContainer;
        this.serverPort = serverPort;
    }

    @Override
    public void run(){
        DatagramSocket datagramSocket;
        try{
            datagramSocket = new DatagramSocket(serverPort);
            while (!datagramSocket.isClosed()){
                byte[] bytes = new byte[1024];
                DatagramPacket datagramPacket = new DatagramPacket(bytes, bytes.length);
                datagramSocket.receive(datagramPacket);
                String message = new String(bytes);

                serverContainer.sendMessageUdp(datagramPacket.getPort(), datagramSocket, message);
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}
