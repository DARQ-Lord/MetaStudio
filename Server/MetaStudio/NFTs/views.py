from collections import deque
from MetaStudio import app,db,account,MetaStudioNFT,Contract
from MetaStudio.Models import UserProfile,Collection
import os
import sys
from pathlib import Path
import requests
from datetime import datetime,date
from flask import Blueprint,render_template,redirect,url_for,request
from flask_login import login_required, current_user,logout_user,login_user
import json
OPENSEA_URL = "https://testnets.opensea.io/assets/avalanche-fuji/{}/{}"

profile_blueprint=Blueprint("Profile",__name__,template_folder="templates",static_folder="static")
def upload_to_ipfs_pinata(image_binary,filename):
    PINATA_BASE_URL = "https://api.nft.storage/"
    endpoint = "upload"
    headers = {
    'Authorization': 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJkaWQ6ZXRocjoweGQ3Mjk5NDcyNDVlNTlhZjNlYTM2MTg3M2ZCMWUxYTdCRGI1MjUzMUEiLCJpc3MiOiJuZnQtc3RvcmFnZSIsImlhdCI6MTY4OTY3NzAzMjY5NSwibmFtZSI6Ik1ldGFTZXVtIn0.U7iPXi9isqLCU4zZeg3PDYRtBOL3JNS6hTMFPSPwPbU',
    'accept': 'application/json',
    'Content-Type': 'image/*'
    }
    # with Path(path).open("rb") as fp:
    #     image_binary = fp.read()
    response = requests.post(
        PINATA_BASE_URL + endpoint,
        data=image_binary,
        headers=headers,
    )
    uri="https://ipfs.io/ipfs/{}?filename={}".format(response.json()['value']["cid"], filename)
    return uri
@profile_blueprint.route("/Authenticate", methods=["GET","POST"])
def Authenticate():
    if request.method=="POST":
        username = request.form.get('username')
        password = request.form.get('password')
        user=UserProfile.query.filter_by(user_email=username,user_password=password).first()
        if user!=None:
            login_user(user, remember=True)
            return redirect(url_for("Profile.Dashboard"))
        else:
            return redirect(url_for("home"))

@profile_blueprint.route("/CreateUser", methods=["GET","POST"])
def CreateUser():
    if request.method=="POST":
        username = request.form.get('username')
        useremail = request.form.get('useremail')
        useraddress = request.form.get('useraddress')
        password = request.form.get('password')
        user=UserProfile(useraddress,username,useremail,password)
        db.session.add(user)
        db.session.commit()
        if user!=None:
            login_user(user, remember=True)
            return redirect(url_for("Profile.Dashboard"))
        else:
            return redirect(url_for("home"))
@profile_blueprint.route("/CreateCollection", methods=["GET","POST"])
def CreateCollection():
    if request.method=="POST":
        UserAddress = request.form.get('UserAddress')
        user=UserProfile.query.filter_by(user_address=UserAddress).first()
        if user!=None:
            collectionToken = request.form.get('collectionToken')
            collectionname = request.form.get('collectionname')
            simple_collection=MetaStudioNFT.deploy(collectionname,collectionToken,{"from": account})
            coll=Collection(collectionname,collectionToken,"",simple_collection.address)
            db.session.add(coll)
            user.Collections.append(coll)
            db.session.commit()
            return {"code":200, "Collection Address":OPENSEA_URL.format(simple_collection.address,"")}
        return {"code":404,"Message":"No Data Found"}
@profile_blueprint.route("/CreateNFT", methods=["GET","POST"])
def CreateNFT():
    if request.method=="POST":
        collectionname = request.form.get('collectionname')
        collection=Collection.query.filter_by(collection_name=collectionname).first()
        if collection!=None:
            nftName = request.form.get('nftName')
            nftDescription = request.form.get('nftDescription')
            user_address = request.form.get('useraddress')
            nftFile = request.files["nftFile"]
            medial_url=upload_to_ipfs_pinata(nftFile.read(),nftFile.filename)
            metadata={
            "image": medial_url,
            "name": nftName,
            "description": nftDescription,
            "external_link": None,
            "animation_url": None,
            "traits": []
            }
            metadata_json=json.dumps(metadata).encode()
            meta_data_uri=upload_to_ipfs_pinata(metadata_json,nftName+".json")
            collectioncontract=Contract(collection.contract_address)
            tx=collectioncontract.safeMint(user_address,meta_data_uri, {"from": account})
            tx.wait(1)
            return {"code":200, "Minted NFT": OPENSEA_URL.format(collectioncontract.address,collectioncontract.tokenCounter()-1)}
        # tx = simple_collectible.safeMint(current_user.user_address,meta_data_uri, {"from": account})
        # tx.wait(1)
        return {"code":404,"Message":"No Data Found"}       
    
def load_user_nfts(collections):
    NFTs=[]
    for collection in collections:
        MetaCanvasNFT = Contract(collection.contract_address)
        count=MetaCanvasNFT.retrieve()
        for i in range(0,count):
            uri=MetaCanvasNFT.retrieve_uri(i)
            response = requests.request("GET", uri)
            nfts_owned=json.loads(response.text)
            nfts_owned["contract_address"]=collection.contract_address
            nfts_owned["token_uri"]=i
            NFTs.append(nfts_owned)
    return NFTs

@profile_blueprint.route("/Dashboard/")
@login_required
def Dashboard():
    myCollections=current_user.Collections
    NFTs=load_user_nfts(myCollections)
    print(NFTs)
    return render_template("dashboard.html",NFTs=NFTs)



@profile_blueprint.route("/Logout", methods=["GET","POST"])
@login_required
def Logout():
    logout_user()
    return redirect(url_for("home"))


@profile_blueprint.route("/AuthenticateVR", methods=["GET","POST"])
def AuthenticateVR():
    if request.method=="POST":
        username = request.form.get('username')
        password = request.form.get('password')
        user=UserProfile.query.filter_by(user_email=username,user_password=password).first()
        if user!=None:
            return {"code":200,"Id":user.id,"User Name":user.user_name,"User Address":user.user_address,"User Email":user.user_email}
    return {"code":404,"Message":"No Data Found"}