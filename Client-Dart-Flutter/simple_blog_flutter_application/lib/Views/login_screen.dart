import 'package:flutter/material.dart';
import '../ViewModels/user_viewmodel.dart';

class LoginScreen extends StatefulWidget {
  const LoginScreen({super.key, required this.title});

  final String title;

  @override
  State<LoginScreen> createState() => _LoginScreenState();
}

class _LoginScreenState extends State<LoginScreen> {
  final usernameController = TextEditingController();
  final passwordController = TextEditingController();
  final loginViewModel = LoginViewModel();

  @override
  void dispose() {
    // Clean up the controller when the widget is disposed.
    usernameController.dispose();
    passwordController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(widget.title),
      ),
      body: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        crossAxisAlignment: CrossAxisAlignment.center,
        children: [
          Padding(
            padding: const EdgeInsets.symmetric(vertical: 10, horizontal: 20),
            child: TextField(
              controller: usernameController,
              decoration: const InputDecoration(
                  border: OutlineInputBorder(),
                  hintText: 'Username',
                  labelText: 'Username'),
            ),
          ),
          Padding(
            padding: const EdgeInsets.symmetric(vertical: 10, horizontal: 20),
            child: TextField(
              controller: passwordController,
              decoration: const InputDecoration(
                  border: OutlineInputBorder(),
                  hintText: 'Password',
                  labelText: 'Password'),
              obscureText: true,
            ),
          ),
          Padding(
            padding: const EdgeInsets.symmetric(vertical: 10, horizontal: 20),
            child: ElevatedButton(
              style: ElevatedButton.styleFrom(
                minimumSize: const Size.fromHeight(60),
              ),
              onPressed: () async {
                final message = await loginViewModel.login(usernameController.text, passwordController.text);
                if (!context.mounted) return;
                if (message.type == 'error') {
                  showDialog(
                    context: context,
                    builder: (context) {
                      return AlertDialog(
                        title: Text(message.type),
                        content: Text(message.content.toString()),
                      );
                    },
                  );
                } else {
                  // Navigator.pushReplacement(
                  //   context, 
                  //   MaterialPageRoute(builder: (context) => const PageListScreen(title: 'Page List',)),
                  // );
                  Navigator.pushReplacementNamed(context, '/page_list');
                }
              },
              child: const Text('Login')
            )
          ),
        ],
      ),
    );
  }
}
