from django.http import HttpResponse
from django.conf import settings

# Create your views here.
def index(request):
	response_html = 'index.html does not exist.'
	with open(settings.STATICFILES_DIRS[0] / 'index.html', 'r') as file:
		response_html = file.read()
	return HttpResponse(response_html)
