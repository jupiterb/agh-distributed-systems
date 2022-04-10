package mountain.trip;

import java.io.IOException;
import java.util.Scanner;
import java.util.concurrent.TimeoutException;

public class Crew {

    public static void main(String[] argv) {
        new Crew(argv[0]).run();
    }

    private final String name;

    public Crew(String name) {
        this.name = name;
        System.out.println("Crew " + name + " joined system");
    }

    public void run() {

        IntermediarySystem intermediarySystem;
        String tag;

        try {
            intermediarySystem = new IntermediarySystem();
            tag = intermediarySystem.initQueue(null, IntermediarySystem.SUPPLY_EXCHANGE, name + ".*");
            intermediarySystem.receiveMessages(tag, name);
        } catch (IOException | TimeoutException e) {
            System.out.println("Cannot set up intermediary system:");
            e.printStackTrace();
            return;
        }

        boolean running = true;
        Scanner scanner = new Scanner(System.in);

        while (running) {
            String [] orders = scanner.nextLine().strip().split(" ");
            for (String order : orders) {
                if (order.equals("exit")) {
                    running = false;
                } else try {
                    intermediarySystem.postMessage(IntermediarySystem.ORDERS_EXCHANGE, order, name);
                } catch (IOException e) {
                    System.out.println("Cannot set up intermediary system:");
                    e.printStackTrace();
                    running = false;
                }
            }
        }

        try {
            intermediarySystem.close();
        } catch (IOException | TimeoutException e) {
            System.out.println("Cannot close intermediary system:");
            e.printStackTrace();
        }
    }
}
