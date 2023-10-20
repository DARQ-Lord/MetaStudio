from MetaStudio import db,UserMixin


class UserProfile(UserMixin,db.Model):
    __tablename__="UserProfile"
    id=db.Column(db.Integer,primary_key=True, autoincrement=True, nullable=False)
    user_name=db.Column(db.Text,unique=True, nullable=False)
    user_address=db.Column(db.Text, unique=True)
    user_email=db.Column(db.Text, nullable=False,server_default='')
    user_password=db.Column(db.Text, nullable=False,server_default='')
    Collections=db.relationship("Collection",backref="UserProfile",primaryjoin="UserProfile.id==Collection.owner_id")
    def __init__(self, user_address,user_name="",user_email="",user_password=""):
        self.user_address=user_address
        self.user_name=user_name
        self.user_email=user_email
        self.user_password=user_password
    def __repr__(self):
        return f"{self.user_address}"


class Collection(db.Model):
    __tablename__="Collection"
    id=db.Column(db.Integer,primary_key=True, autoincrement=True, nullable=False)
    collection_name=db.Column(db.Text)
    collection_token=db.Column(db.Text)
    owner_id=db.Column(db.Integer,db.ForeignKey('UserProfile.id'))
    collection_abi=db.Column(db.Text, nullable=False,server_default='')
    contract_address=db.Column(db.Text, nullable=False,server_default='')
    def __init__(self, collection_name="",collection_token="",collection_abi="",contract_address=""):
        self.collection_name=collection_name
        self.collection_token=collection_token
        self.collection_abi=collection_abi
        self.contract_address=contract_address
    def __repr__(self):
        return f"{self.collection_name}"