class Page {
  int ?id;
  int userId;
  String title;
  String url;
  String ?content;
  int ?datePublished;
  int ?dateModified;

  Page({
    this.id, 
    required this.userId, 
    required this.title, 
    required this.url, 
    this.content, 
    this.datePublished, 
    this.dateModified
  });

  factory Page.fromJson(Map<String, dynamic> json) {
    return Page(
      id: json['id'],
      userId: json['user_id'],
      title: json['title'],
      url: json['url'],
      content: json['content'],
      datePublished: json['date_published'],
      dateModified: json['date_modified'],
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'user_id': userId,
      'title': title,
      'url': url,
      'content': content,
      'date_published': datePublished,
      'date_modified': dateModified,
    };
  }
}