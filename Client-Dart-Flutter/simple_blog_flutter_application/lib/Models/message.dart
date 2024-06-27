class Message {
    final String type;
    final Object content;

    Message({required this.type, required this.content});

    factory Message.fromJson(Map<String, dynamic> json) {
      return Message(
        type: json['type'],
        content: json['content'],
      );
    }
}