package server;

import java.io.IOException;
import java.net.ServerSocket;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

public class ListenerServerTCP extends Thread {

    private final ServerContainer serverContainer;
    private final int serverPort;
    private final int maxClients;

    public ListenerServerTCP(ServerContainer serverContainer, int serverPort, int maxClients){
        this.serverContainer = serverContainer;
        this.serverPort = serverPort;
        this.maxClients = maxClients;
    }

    @Override
    public void run(){
        ServerSocket serverSocket;
        try {
            serverSocket = new ServerSocket(serverPort);

            ExecutorService executor = Executors.newFixedThreadPool(maxClients);
            while (!serverSocket.isClosed()){
                executor.submit(new ClientHandler(serverContainer, serverSocket.accept()));
            }
        }
        catch (IOException e) {
            e.printStackTrace();
        }
    }
}
