package client;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.nio.file.Files;
import java.nio.file.Paths;

public class ClientInput extends Thread {

    // TCP communication
    private final PrintWriter out;

    // UDP communication
    private final DatagramSocket datagramSocket;
    private final int serverPort;
    private InetAddress udpAddress;

    private final int multicastPort;
    private InetAddress multicastAddress;

    public ClientInput(PrintWriter out, DatagramSocket datagramSocket, int serverPort, String udpAddress, int multicastPort, String multicastAddress){
        this.out = out;
        this.serverPort = serverPort;
        this.datagramSocket = datagramSocket;
        // Multicast communication
        this.multicastPort = multicastPort;

        try {
            this.udpAddress = InetAddress.getByName(udpAddress);
            this.multicastAddress = InetAddress.getByName(multicastAddress);
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    @Override
    public void run(){
        BufferedReader br = new BufferedReader(new InputStreamReader(System.in));
        while (true){
            String input = null;
            try {
                input = br.readLine();
            } catch (IOException e) {
                e.printStackTrace();
            }

            assert input != null;
            if (sessionFinished(input)) {
                break;
            } else try {
                handleMessage(input);
            } catch (IOException e) {
                e.printStackTrace();
            }
        }
        System.out.println("Chat finished");
    }

    private boolean sessionFinished(String input){
        return input.toLowerCase().startsWith("exit");
    }

    private void handleMessage(String input) throws IOException {
        if (input.startsWith("U ")){
            input = getDataFromFile(input.substring(2));
            byte[] msgBytes = input.getBytes();
            DatagramPacket packet = new DatagramPacket(msgBytes, msgBytes.length, udpAddress, serverPort);
            datagramSocket.send(packet);
        } else if (input.startsWith("M ")){
            input = getDataFromFile(input.substring(2));
            byte[] msgBytes = input.getBytes();
            DatagramPacket packet = new DatagramPacket(msgBytes, msgBytes.length, multicastAddress, multicastPort);
            datagramSocket.send(packet);
        } else {
            out.println(input);
        }
    }

    private String getDataFromFile(String fileName){
        String message = "";
        try {
            message = new String(Files.readAllBytes(Paths.get(fileName)));
            message = "\n" + message;
        } catch (IOException e) {
            e.printStackTrace();
        }
        return message;
    }
}
