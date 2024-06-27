import 'package:http/http.dart' as http;
import 'dart:convert';

import '../Models/user.dart';
import '../Models/message.dart';

class LoginViewModel {
  
  final apiUrl = 'http://localhost:8080/api';

  Future<Message> login(String username, String password) async {
    var loginUrl = '$apiUrl/user/login/?username=$username&password=$password';
    final response = await http.get(
      Uri.parse(loginUrl), 
    );
    if (response.statusCode == 200) {
      var msg = Message.fromJson(jsonDecode(response.body));
      User.username = username;
      User.token = msg.content.toString();
      return msg;
    } else {
      return Message(type: 'error', content: '$response.statusCode: $response.body');
    }
  }
}