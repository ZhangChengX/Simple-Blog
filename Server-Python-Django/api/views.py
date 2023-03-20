from django.shortcuts import render
from django.http import HttpResponse, JsonResponse, QueryDict
from django.conf import settings
from django.apps import apps
from django.views.decorators.csrf import csrf_exempt
from datetime import datetime
import jwt

Page = apps.get_model('page', 'Page')
User = apps.get_model('user', 'User')

# Create your views here.
@csrf_exempt
def page_restful(request):
	request_data = request.GET
	if 'token' not in request_data or not verify(request_data['token']):
		return JsonResponse({'type': 'error', 'content': 'Please login.'})
	if request.method == 'PUT':
		return put(request)
	elif request.method == 'POST':
		return post(request)
	elif request.method == 'DELETE':
		return delete(request)
	else:
		return get(request)

def get(request):
	request_data = request.GET
	if 'token' not in request_data or not verify(request_data['token']):
		return JsonResponse({'type': 'error', 'content': 'Please login.'})
	if 'all' == request_data['id']:
		page_list = Page.objects.all()
		response_data = []
		for page_detail in page_list:
			response_data.append({
				'id': page_detail.id,
				'user_id': page_detail.user_id,
				'url': page_detail.url,
				'title': page_detail.title,
				'content': page_detail.content,
				'date_published': page_detail.date_published,
				'date_modified': page_detail.date_modified
			})
	else:
		page_detail = Page.objects.get(id=request_data['id'])
		response_data = [{
			'id': page_detail.id,
			'user_id': page_detail.user_id,
			'url': page_detail.url,
			'title': page_detail.title,
			'content': page_detail.content,
			'date_published': page_detail.date_published,
			'date_modified': page_detail.date_modified
		}]
	return JsonResponse({'type': 'success', 'content': response_data})

def put(request):
	request_data = request.GET
	if 'token' not in request_data or not verify(request_data['token']):
		return JsonResponse({'type': 'error', 'content': 'Please login.'})
	data = QueryDict(request.body)
	page_detail = Page.objects.get(id=data.get('id'))
	if data.get('user_id'): page_detail.user_id = data.get('user_id')
	if data.get('url'): page_detail.url = data.get('url')
	if data.get('title'): page_detail.title = data.get('title')
	if data.get('content'): page_detail.content = data.get('content')
	if data.get('date_published'): page_detail.date_published = data.get('date_published')
	if data.get('date_modified'): page_detail.date_modified = data.get('date_modified')
	page_detail.save()
	return JsonResponse({'type': 'success', 'content': 'Page updated successfully.'})

def post(request):
	request_data = request.GET
	if 'token' not in request_data or not verify(request_data['token']):
		return JsonResponse({'type': 'error', 'content': 'Please login.'})
	data = request.POST
	new_page = Page(
		user_id=data.get('user_id'),
		url=data.get('url'),
		title=data.get('title'),
		content=data.get('content'),
		date_published=data.get('date_published'),
		date_modified=data.get('date_modified')
		)
	new_page.save()
	if new_page.id:
		return JsonResponse({'type': 'success', 'content': 'Page added successfully.'})
	else:
		return JsonResponse({'type': 'error', 'content': 'Unknown error.'})

def delete(request):
	request_data = request.GET
	if 'token' not in request_data or not verify(request_data['token']):
		return JsonResponse({'type': 'error', 'content': 'Please login.'})
	Page.objects.filter(id=request_data['id']).delete()
	return JsonResponse({'type': 'success', 'content': 'Page deleted successfully.'})

@csrf_exempt
def login(request):
	request_data = request.GET
	try:
		user_data = User.objects.get(username=request_data['username'], password=request_data['password'])
	except User.DoesNotExist:
		return JsonResponse({'type': 'error', 'content': 'Invalid Credentials provided.'})

	token = jwt.encode({'user_id': user_data.id, 'expiry': datetime.timestamp(datetime.utcnow()) + 60 * 3}, settings.SECRET_KEY, algorithm='HS256')
	return JsonResponse({'type': 'success', 'content': token})

@csrf_exempt
def logout(request):
	request_data = request.GET
	if 'token' not in request_data or not verify(request_data['token']):
		return JsonResponse({'type': 'error', 'content': 'Please login.'})
	return JsonResponse({'type': 'success', 'content': refresh_token(request_data['token'], datetime.timestamp(datetime.utcnow()) - 3600)})

def verify(token):
	try:
		decoded_token = jwt.decode(token, settings.SECRET_KEY, algorithms=['HS256'])
	except:
		return False
	if decoded_token['expiry'] < datetime.timestamp(datetime.utcnow()):
		print('Token expired.' + 
			' Token time: ' + datetime.fromtimestamp(decoded_token['expiry']).strftime("%Y-%m-%d %H:%M:%S") +
			' Current time: ' + datetime.utcnow().strftime("%Y-%m-%d %H:%M:%S"))
		return False
	return True

def refresh_token(token, expiry):
	try:
		decoded_token = jwt.decode(token, settings.SECRET_KEY, algorithms=['HS256'])
	except:
		return token
	decoded_token['expiry'] = expiry
	token = jwt.encode(decoded_token, settings.SECRET_KEY, algorithm='HS256')
	return token