package server;

import java.net.DatagramSocket;
import java.util.HashSet;
import java.util.concurrent.locks.Lock;
import java.util.concurrent.locks.ReentrantLock;

public class ServerContainer {

    private final HashSet<ClientHandler> clients = new HashSet<>();
    private static final Lock clientLock = new ReentrantLock();

    public void addClient(ClientHandler client){
        clientLock.lock();
        clients.add(client);
        clientLock.unlock();
    }

    public void removeClient(ClientHandler client){
        clientLock.lock();
        clients.remove(client);
        clientLock.unlock();
    }

    public boolean isNameAvailable(String name){

        clientLock.lock();

        for (var client : clients){
            if (client.getClientName() != null && client.getClientName().equals(name)){
                clientLock.unlock();
                return false;
            }
        }
        clientLock.unlock();
        return true;
    }

    public void sendMessageFrom(String name, String message){

        message = name + " send message: " + message;

        clientLock.lock();

        for (var client : clients){
            if (client.getClientName() != null && !client.getClientName().equals(name)){
                client.sendMessageTCP(message);
            }
        }

        clientLock.unlock();
    }

    public void sendMessageUdp(int sourcePort, DatagramSocket datagramSocket, String message){
        message = " udp message: " + message;

        clientLock.lock();

        for (var client : clients){
            client.sendMessageUDP(message, datagramSocket, sourcePort);
        }

        clientLock.unlock();
    }
}
