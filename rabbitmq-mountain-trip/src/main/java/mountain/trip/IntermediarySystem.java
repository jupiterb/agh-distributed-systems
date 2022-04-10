package mountain.trip;

import com.rabbitmq.client.*;

import java.io.IOException;
import java.nio.charset.StandardCharsets;
import java.util.concurrent.TimeoutException;

public class IntermediarySystem {

    public static final String ORDERS_EXCHANGE = "orders_exchange";
    public static final String SUPPLY_EXCHANGE = "supply_exchange";

    private static final String HOST = "localhost";

    private final Channel channel;
    private final Connection connection;

    private int nextReplyId = 0;

    public IntermediarySystem() throws IOException, TimeoutException {
        ConnectionFactory connectionFactory = new ConnectionFactory();
        connectionFactory.setHost(HOST);
        connection = connectionFactory.newConnection();
        channel = connection.createChannel();

        channel.exchangeDeclare(ORDERS_EXCHANGE, BuiltinExchangeType.TOPIC);
        channel.exchangeDeclare(SUPPLY_EXCHANGE, BuiltinExchangeType.TOPIC);
    }

    public String initQueue(String queueName, String exchangeName, String routingKey) throws IOException {
        if (queueName == null) {
            queueName = channel.queueDeclare().getQueue();
        } else{
            channel.queueDeclare(queueName, true, false, false, null);
        }
        channel.queueBind(queueName, exchangeName, routingKey);
        return queueName;
    }

    public void receiveMessages(String tag, String name) throws IOException {
        channel.basicConsume(tag, false, "", new DefaultConsumer(channel) {
           @Override
           public void handleDelivery(String tag, Envelope envelope, AMQP.BasicProperties properties, byte[] body) throws IOException {
               int id = Integer.parseInt(envelope.getRoutingKey().split("\\.")[1]);
               String [] message = new String(body, StandardCharsets.UTF_8).split("\\.");
               String sender = message[0];
               String stuff = message[1];

               System.out.println(name + " received " + stuff + " (id = " + id + ") from " + sender);
               channel.basicAck(envelope.getDeliveryTag(), false);
           }
        });
    }

    public void replayForMessages(String stuff, String name) throws IOException {
        channel.basicConsume(stuff, false, "", new DefaultConsumer(channel) {
            @Override
            public void handleDelivery(String tag, Envelope envelope, AMQP.BasicProperties properties, byte[] body) throws IOException {
                String replayTo = new String(body, StandardCharsets.UTF_8);
                System.out.println(name + " reply to " + replayTo + " with " + stuff + " (id = " + nextReplyId + ")");

                String message = name + "." + stuff;
                postMessage(SUPPLY_EXCHANGE, replayTo + "." + nextReplyId, message);

                channel.basicAck(envelope.getDeliveryTag(), false);
                nextReplyId ++;
            }
        });
    }

    public void postMessage(String exchangeName, String tag, String message) throws IOException {
        channel.basicPublish(exchangeName, tag, null, message.getBytes());
    }

    public void close() throws IOException, TimeoutException {
        channel.close();
        connection.close();
    }
}
