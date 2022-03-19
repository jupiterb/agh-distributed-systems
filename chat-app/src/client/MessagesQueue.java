package client;

import java.util.LinkedList;
import java.util.concurrent.locks.Lock;
import java.util.concurrent.locks.ReentrantLock;

public class MessagesQueue {

    private final LinkedList<String> messages = new LinkedList<>();
    private static final Lock queueLock = new ReentrantLock();

    public void push(String message){
        queueLock.lock();
        messages.addLast(message);
        queueLock.unlock();
    }

    public String pop(){
        queueLock.lock();
        String message = messages.pop();
        queueLock.unlock();
        return message;
    }

    public boolean hasAny() {
        return !messages.isEmpty();
    }
}
