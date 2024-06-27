import 'package:flutter/material.dart';

import '../ViewModels/page_viewmodel.dart';
import '../Views/page_edit_screen.dart';
import '../Models/message.dart';
import '../Models/user.dart';
import '../Models/page.dart' as models;


class PageListScreen extends StatefulWidget {
  const PageListScreen({super.key, required this.title});

  final String title;

  @override
  State<PageListScreen> createState() => _PageListScreenState();
}

class _PageListScreenState extends State<PageListScreen> {

  final pageViewModel = PageViewModel();
  List<models.Page> pages = [];

  @override
  void initState() {
    super.initState();
    _fetchPages();  // Fetch pages when the screen initializes
  }

  void _showAlertDialog(Message message) {
    showDialog(
      context: context,
      builder: (context) {
        return AlertDialog(
          title: Text(message.type),
          content: Text(message.content.toString()),
        );
      },
    );
  }

  Future<void> _fetchPages() async {
    final msg = await pageViewModel.get(null, User.token);
    if (msg.type == 'success') {
      setState(() {
        pages = msg.content as List<models.Page>;
      });
    } else {
      _showAlertDialog(msg);
    }
  }

  Future<void> _deletePage(int id) async {
    final msg = await pageViewModel.delete(id, User.token);
    if (msg.type == 'success') {
      _fetchPages();
    } else {
      _showAlertDialog(msg);
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Row(children: [
          Text(widget.title),
          const Padding(padding: EdgeInsets.only(right: 60)),
          TextButton(
            onPressed: () {
              Navigator.pushNamed(context, '/page_add').then((_) => setState(() {_fetchPages();}));
              // Navigator.push(
              //   context,
              //   MaterialPageRoute(builder: (context) => const PageEditScreen(title: 'Add New',)),
              // ).then((_) => setState(() {_fetchPages();}));
            }, 
            child: const Text('Add New')
          ),
          TextButton(
            onPressed: () {
              Navigator.pushReplacementNamed(context, '/');
            }, 
            child: Text('${User.username} Logout')
          ),
        ],),
        actions: [
          IconButton(onPressed: () {
              Navigator.pushNamed(context, '/page_add').then((_) => setState(() {_fetchPages();}));
            }, 
            icon: const Icon(Icons.add_circle_outline)
          )
        ],
      ),
      body: pages.isEmpty
        ? const Center(child: CircularProgressIndicator())
        : DataTable(
            showBottomBorder: true,
            columns: const [
              DataColumn(label: Text("ID")),
              DataColumn(label: Text("Title")),
              DataColumn(label: Text("Author")),
              DataColumn(label: Text("Date")),
              DataColumn(label: Center(child: Text("Action", textAlign: TextAlign.center,))),
            ], 
            rows: pages.map((page) => 
              DataRow(cells: [
                DataCell(Text(page.id.toString())),
                DataCell(Text(page.title)),
                DataCell(Text(page.userId.toString())),
                DataCell(Text(DateTime.fromMillisecondsSinceEpoch(page.dateModified ?? 0).toString() )),
                DataCell(
                  Row(children: [
                    Expanded(
                      flex: 5, 
                      child: TextButton(
                        onPressed: () {
                          Navigator.push(
                            context,
                            MaterialPageRoute(builder: (context) => PageEditScreen(title: 'Page Edit', id: page.id,)),
                          )
                          .then((_) => setState(() {_fetchPages();})); // Refreash page when go back from PageEditScreen
                        },
                        child: const Text("Edit"),
                      ),
                    ),
                    Expanded(
                      flex: 5, 
                      child: TextButton(
                        onPressed: () {
                          _deletePage(page.id!);
                        },
                        child: const Text("Remove"),
                      ),
                    ),
                  ],)
                ),
              ])
            ).toList(),
          )
    );
  }
}