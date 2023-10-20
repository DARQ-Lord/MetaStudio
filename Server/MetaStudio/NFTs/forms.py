from flask_wtf import FlaskForm
from wtforms import StringField,SubmitField,SelectField,validators

class CreateNFT(FlaskForm):
    nft_name=StringField("Enter the Subject Name",validators=[validators.required()])
    submit=SubmitField("Add Subject")
    