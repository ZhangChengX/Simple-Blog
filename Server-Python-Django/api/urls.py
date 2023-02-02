from django.urls import path

from . import views

urlpatterns = [
    path('page/', views.page_restful, name='page_restful'),
    path('user/login/', views.login, name='user_login'),
    path('user/logout/', views.logout, name='user_logout'),
]