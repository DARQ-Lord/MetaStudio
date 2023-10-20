from flask_restful import Resource, Api


from MetaStudio.Models import UserProfile,Collection


class MyCollection(Resource):
    def get(self,address):
        user=UserProfile.query.filter_by(user_address=address).first()
        if user!= None:
            collections=[]
            for collection in list(user.Collections):
                collections.append({"id":collection.id,"Collection Address":collection.contract_address,"Collection Name":collection.collection_name,"Collection Token":collection.collection_token})
            return {"code":200,"Collections":collections}
        return {"code":404,"Message":"No Data Found"},404
