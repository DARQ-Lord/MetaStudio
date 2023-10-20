from brownie import *
from os.path import join

import os,sys
from flask import Flask,render_template, redirect,url_for
from flask_sqlalchemy import SQLAlchemy
from flask_restful import Api
from flask_login import LoginManager,UserMixin

direc=os.path.abspath(os.path.dirname(__file__))
app=Flask(__name__)
app.config["SQLALCHEMY_DATABASE_URI"]="sqlite:///"+os.path.join(direc,"data.sqlite")
app.config["SQLALCHEMY_TRACK_MODIFICATIONS"]=False
SECRET_KEY = os.urandom(32)
app.config['SECRET_KEY'] = SECRET_KEY
db=SQLAlchemy(app)
api=Api(app=app)
app.app_context().push()
project_path="./MetaStudio/static/BrownieProject"
p = project.load(project_path, name="MetaStudioNFT")
p.load_config()
from brownie import network,Contract
network.connect('fuji')
sample_token_uri = "https://ipfs.io/ipfs/Qmd9MCGtdVz2miNumBHDbvj8bigSgTwnr4SbyH6DNnpWdt?filename=0-PUG.json"
OPENSEA_URL = "https://testnets.opensea.io/assets/goerli/{}/{}"

from brownie.project.MetaStudioNFT import MetaStudioNFT
account = accounts.add("8ad2efe017767f747b402df39b0146c3d99ce2910b5708d7bff12084f8f00185")

from MetaStudio.NFTs.views import profile_blueprint

app.register_blueprint(profile_blueprint,url_prefix="/Profile/")


from MetaStudio.REST.RESTful import MyCollection

api.add_resource(MyCollection, "/rest/mycollection/<string:address>/")