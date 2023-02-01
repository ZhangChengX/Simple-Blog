from django.urls import path

from . import views

urlpatterns = [
    path('', views.list, name='page_list'),
    path('<str:page_id>/', views.detail, name='page_detail'),
]