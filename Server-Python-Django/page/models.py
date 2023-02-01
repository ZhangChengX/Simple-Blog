from django.db import models

# Create your models here.
class Page(models.Model):
	user = models.ForeignKey('user.User', on_delete=models.CASCADE)
	url = models.CharField(max_length=512)
	title = models.CharField(max_length=512)
	content = models.CharField(max_length=1024)
	# date_published = models.DateTimeField(auto_now_add=True)
	# date_modified = models.DateTimeField(auto_now=True)
	date_published = models.IntegerField()
	date_modified = models.IntegerField()

	def __str__(self):
		return self.title

	class Meta:
		db_table = 'page'