import 'package:flutter/material.dart';
import '../ViewModels/page_viewmodel.dart';
import '../Views/page_list_screen.dart';
import '../Models/message.dart';
import '../Models/user.dart';
import '../Models/page.dart' as models;

class PageEditScreen extends StatefulWidget {
  const PageEditScreen({super.key, required this.title, this.id});

  final String title;
  final int? id;

  @override
  State<PageEditScreen> createState() => _PageEditScreenState();
}

class _PageEditScreenState extends State<PageEditScreen> {

  final titleController = TextEditingController();
  final urlController = TextEditingController();
  final contentController = TextEditingController();
  final pageViewModel = PageViewModel();
  late models.Page page;

  @override
  void initState() {
    super.initState();
    if(widget.id == null) {
      page = models.Page(userId: 0, title: "", url: "");
      page.datePublished = DateTime.now().millisecondsSinceEpoch;
    } else {
      _loadPage();
    }
    titleController.addListener(() {
      page.title = titleController.text;
    });
    urlController.addListener(() {
      page.url = urlController.text;
    });
    contentController.addListener(() {
      page.content = contentController.text;
    });
  }

  void _showAlertDialog(Message message) {
    showDialog(
      context: context,
      builder: (context) {
        return AlertDialog(
          title: Text(message.type),
          content: Text(message.content.toString()),
          actions: <Widget>[
            TextButton(
              child: const Text('OK'),
              onPressed: () {
                Navigator.pop(context); // Dismiss popup dialog
                Navigator.pop(context); // Close current screen
              },
            ),
          ],
        );
      },
    );
  }

  Future<void> _loadPage() async {
    final msg = await pageViewModel.get(widget.id, User.token);
    if (msg.type == 'success') {
      // setState(() {
        final pages = msg.content as List<models.Page>;
        page = pages[0];
        titleController.text = page.title;
        urlController.text = page.url;
        contentController.text = page.content ?? '';
      // });
    } else {
      _showAlertDialog(msg);
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(widget.title),
        actions: [
          IconButton(onPressed: () {
            Navigator.pushReplacement(
              context,
              MaterialPageRoute(builder: (context) => const PageListScreen(title: 'Page List',)),
            );
          }, 
          icon: const Icon(Icons.list)
          )
        ],
      ),
      body: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        crossAxisAlignment: CrossAxisAlignment.center,
        children: [
          Padding(
            padding: const EdgeInsets.symmetric(vertical: 10, horizontal: 20),
            child: TextField(
              controller: titleController,
              decoration: const InputDecoration(
                  border: OutlineInputBorder(),
                  hintText: 'Title',
                  labelText: 'Title'),
            ),
          ),
          Padding(
            padding: const EdgeInsets.symmetric(vertical: 10, horizontal: 20),
            child: TextField(
              controller: urlController,
              decoration: const InputDecoration(
                  border: OutlineInputBorder(),
                  hintText: 'URL',
                  labelText: 'URL'),
            ),
          ),
          Padding(
            padding: const EdgeInsets.symmetric(vertical: 10, horizontal: 20),
            child: TextField(
              controller: contentController,
              maxLines: 10,
              decoration: const InputDecoration(
                  border: OutlineInputBorder(),
                  hintText: 'Enter page content here...',
                  labelText: 'Content'),
            ),
          ),
          Padding(
            padding: const EdgeInsets.symmetric(vertical: 10, horizontal: 20),
            child: ElevatedButton(
              style: ElevatedButton.styleFrom(
                minimumSize: const Size.fromHeight(60),
              ),
              onPressed: () async {
                page.dateModified = DateTime.now().millisecondsSinceEpoch;
                Message msg;
                if(widget.id == null) {
                  msg = await pageViewModel.post(page, User.token);
                } else {
                  msg = await pageViewModel.put(page, User.token);
                }
                _showAlertDialog(msg);
              },
              child: const Text('Submit')
            )
          )
        ],
      )
    );
  }
}