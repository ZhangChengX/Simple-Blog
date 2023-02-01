from django.shortcuts import render, get_object_or_404
from django.http import Http404
from django.apps import apps
from .models import Page

User = apps.get_model('user', 'User')

# Create your views here.
def list(request):
	page_list = Page.objects.all()
	data = {
		'title': 'Page List',
		'page_list': page_list
	}
	return render(request, 'page/list.html', data)

def detail(request, page_id):
	if page_id.isdigit():
		# page_detail = Page.objects.get(id=int(page_id))
		page_detail = get_object_or_404(Page, id=int(page_id))
	else:
		try:
			page_detail = Page.objects.get(url=page_id)
		except Page.DoesNotExist:
			raise Http404("Page does not exist")
	date = page_detail.__dict__
	if date['date_modified'] and date['date_modified'] != 0:
		date['date'] = date['date_modified']
	else:
		date['date'] = date['date_published']
	if date['user_id'] <= 0:
		user_id = 1
	else:
		user_id = date['user_id']
	date['username'] = User.objects.get(id=user_id).username
	return render(request, 'page/detail.html', date)
