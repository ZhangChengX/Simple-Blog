from django.db import models

# Create your models here.
class User(models.Model):
	username = models.CharField(max_length=256)
	password = models.CharField(max_length=256)

	def __str__(self):
		return self.username

	class Meta:
		db_table = 'user'