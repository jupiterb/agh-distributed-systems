package server;

import java.io.IOException;
import java.io.PrintWriter;
import java.net.*;
import java.util.NoSuchElementException;
import java.util.Scanner;

public class ClientHandler extends Thread {

    private String name;

    private final ServerContainer serverContainer;
    private final Socket socket;

    private Scanner input;
    private PrintWriter output;

    public ClientHandler(ServerContainer serverContainer, Socket socket){
        this.serverContainer = serverContainer;
        this.socket = socket;

        try {
            input = new Scanner(socket.getInputStream());
            output = new PrintWriter(socket.getOutputStream(), true);
        }
        catch (IOException e) {
            e.printStackTrace();
        }
    }

    @Override
    public void run(){
        serverContainer.addClient(this);
        while (true){
            try {
                String input = this.input.nextLine();

                if (input.toLowerCase().startsWith("exit")) {
                    break;
                } else if (!handleLogin(input)) {
                    trySndMessageToOthers(input);
                }

            } catch (NoSuchElementException e) {
                break;
            }
        }
        serverContainer.removeClient(this);
        if (name != null && !name.isEmpty()){
            System.out.println("User " + name + " left the server");
        }
    }

    public String getClientName(){
        return name;
    }

    public void sendMessageTCP(String message){
        output.println(message);
    }

    public void sendMessageUDP(String message, DatagramSocket datagramSocket, int sourcePort){
        try {
            int port = socket.getPort();
            if (port != sourcePort){
                InetAddress address = InetAddress.getByName("localhost");
                byte[] messageBytes = message.getBytes();
                datagramSocket.send(new DatagramPacket(messageBytes, messageBytes.length, address, port));
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    private void trySndMessageToOthers(String message){
        if (name == null){
            output.println("You cannot send message if you do not have login set");
        } else {
            serverContainer.sendMessageFrom(name, message);
        }
    }

    private boolean handleLogin(String message){
        if (message.toLowerCase().startsWith("login ")){
            String proposedName = message.split(" ")[1];
            if (serverContainer.isNameAvailable(proposedName)){
                name = proposedName;
                System.out.println("User " + name + " joined to the server");
                output.println("You are logged as " + name);
            } else {
                output.println("Proposed login is not available");
            }
            return true;
        } else {
            return false;
        }
    }
}
