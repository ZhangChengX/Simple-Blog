import 'package:http/http.dart' as http;
import 'dart:convert';

import '../Models/page.dart';
import '../Models/message.dart';

class PageViewModel {

  final apiUrl = 'http://localhost:8080/api';

  Future<Message> get(int ?id, String token) async {
    var url = '$apiUrl/page/?id=$id&token=$token';
    if (id == null) {
      url = '$apiUrl/page/?id=all&token=$token';
    }
    try {
      final response = await http.get(
        Uri.parse(url),
      );
      if (response.statusCode == 200) {
        final responseBody = jsonDecode(response.body);
        if (responseBody['type'] == 'success') {
          final List<dynamic> responseData = responseBody['content'];
          final pages = responseData.map((json) => Page.fromJson(json)).toList();
          return Message(type: 'success', content: pages);
        } else {
          return Message(type: 'error', content: responseBody['content']);
        }
      } else {
        return Message(type: 'error', content: '$response.statusCode: $response.body');
      }
    } catch (e) {
      return Message(type: 'error', content: 'Error fetching pages: $e');
    }
  }

  Future<Message> post(Page page, String token) async {
    var url = '$apiUrl/page/?token=$token';
    try {
      final response = await http.post(
        Uri.parse(url),
        body: jsonEncode(page),
      );
      if (response.statusCode == 200) {
        final responseBody = jsonDecode(response.body);
        if (responseBody['type'] == 'success') {
          return Message(type: 'success', content: responseBody['content']);
        } else {
          return Message(type: 'error', content: responseBody['content']);
        }
      } else {
        return Message(type: 'error', content: '$response.statusCode: $response.body');
      }
    } catch(e) {
      return Message(type: 'error', content: 'Error creating page: $e');
    }
  }

  Future<Message> put(Page page, String token) async {
    var pageId = page.id.toString();
    var url = '$apiUrl/page/?id=$pageId&token=$token';
    try {
      final response = await http.put(
        Uri.parse(url),
        body: jsonEncode(page),
      );
      if (response.statusCode == 200) {
        final responseBody = jsonDecode(response.body);
        if (responseBody['type'] == 'success') {
          return Message(type: 'success', content: responseBody['content']);
        } else {
          return Message(type: 'error', content: responseBody['content']);
        }
      } else {
        return Message(type: 'error', content: '$response.statusCode: $response.body');
      }
    } catch(e) {
      return Message(type: 'error', content: 'Error updating page: $e');
    }
  }

  Future<Message> delete(int id, String token) async {
    var url = '$apiUrl/page/?id=$id&token=$token';
    try {
      final response = await http.delete(
        Uri.parse(url),
      );
      if (response.statusCode == 200) {
        final responseBody = jsonDecode(response.body);
        if (responseBody['type'] == 'success') {
          return Message(type: 'success', content: responseBody['content']);
        } else {
          return Message(type: 'error', content: responseBody['content']);
        }
      } else {
        return Message(type: 'error', content: '$response.statusCode: $response.body');
      }
    } catch (e) {
      return Message(type: 'error', content: 'Error deleting pages: $e');
    }
  }
}