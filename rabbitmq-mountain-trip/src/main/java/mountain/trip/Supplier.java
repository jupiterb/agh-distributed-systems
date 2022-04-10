package mountain.trip;

import java.io.IOException;
import java.util.Arrays;
import java.util.HashSet;
import java.util.Scanner;
import java.util.concurrent.TimeoutException;

public class Supplier {

    public static void main(String[] argv) {
        new Supplier(argv[0], Arrays.copyOfRange(argv, 1, argv.length)).run();
    }

    private final String name;
    private final HashSet<String> offerSet;

    public Supplier(String name, String[] offers) {
        this.name = name;
        this.offerSet = new HashSet<>(Arrays.asList(offers));
        System.out.println("Supplier " + name + " joined system and offers:");
        this.offerSet.forEach( (offerName) -> System.out.println("* " + offerName) );
    }

    public void run() {

        IntermediarySystem intermediarySystem;

        try {
            intermediarySystem = new IntermediarySystem();
            for (String offer : offerSet) {
                intermediarySystem.initQueue(offer, IntermediarySystem.ORDERS_EXCHANGE, offer);
                intermediarySystem.replayForMessages(offer, name);
            }
        } catch (IOException | TimeoutException e) {
            System.out.println("Internal system error:");
            e.printStackTrace();
            return;
        }

        boolean running = true;
        Scanner scanner = new Scanner(System.in);

        while (running) {
            String [] commands = scanner.nextLine().strip().split(" ");
            for (String command : commands)
                if (command.equals("exit")) {
                    running = false;
                    break;
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