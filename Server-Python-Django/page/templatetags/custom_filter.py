from django import template
from datetime import datetime

register = template.Library()

@register.filter(name='timestamp_to_datetime')
def timestamp_to_datetime(timestamp):
	if not timestamp:
		timestamp = 0
	return datetime.utcfromtimestamp(timestamp / 1e3)