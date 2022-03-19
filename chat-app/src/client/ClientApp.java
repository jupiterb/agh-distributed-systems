package client;

import java.io.IOException;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.net.MulticastSocket;
import java.net.Socket;

public class ClientApp {

    public static void main(String[] args){

        String serverAddress = "127.0.0.1";
        int serverPort = 12345;

        String multicastAddress = "227.1.2.3";
        int multicastPort = 12347;

        MessagesQueue messagesReceivedQueue = new MessagesQueue();

        Socket socket;
        DatagramSocket datagramSocket;
        MulticastSocket multicastSocket;

        try {
            socket = new Socket(serverAddress, serverPort);
            datagramSocket = new DatagramSocket(socket.getLocalPort());

            multicastSocket = new MulticastSocket(multicastPort);
            InetAddress multicastGroup = InetAddress.getByName(multicastAddress);
            multicastSocket.joinGroup(multicastGroup);

        } catch (IOException e) {
            e.printStackTrace();
            return;
        }

        ListenerTCP listenerTCP = new ListenerTCP(messagesReceivedQueue, socket);
        listenerTCP.start();

        ListenerUDP listenerUDP = new ListenerUDP(messagesReceivedQueue, datagramSocket);
        listenerUDP.start();

        ListenerMulticast listenerMulticast = new ListenerMulticast(messagesReceivedQueue, multicastSocket);
        listenerMulticast.start();

        ClientInput input = new ClientInput(listenerTCP.getOut(),
                datagramSocket, serverPort, serverAddress,
                multicastPort, multicastAddress);

        ClientOutput output = new ClientOutput(messagesReceivedQueue);

        input.start();
        output.start();

        try{
            input.join();

            output.interrupt();

            listenerTCP.interrupt();
            socket.close();

            listenerUDP.interrupt();
            datagramSocket.close();

            listenerMulticast.interrupt();
            multicastSocket.close();

        } catch (InterruptedException | IOException e) {
            e.printStackTrace();
        }
    }
}
