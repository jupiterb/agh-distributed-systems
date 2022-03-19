package server;

public class ServerApp {

    public static void main(String[] args){

        int serverPort = 12345;
        int maxClients = 25;

        ServerContainer serverContainer = new ServerContainer();

        ListenerServerTCP listenerServerTCP = new ListenerServerTCP(serverContainer, serverPort, maxClients);
        ListenerServerUDP listenerServerUDP = new ListenerServerUDP(serverContainer, serverPort);

        listenerServerTCP.start();
        listenerServerUDP.start();

        try{
            listenerServerTCP.join();
            listenerServerUDP.join();
        } catch (InterruptedException interruptedException) {
            interruptedException.printStackTrace();
        }
    }
}
