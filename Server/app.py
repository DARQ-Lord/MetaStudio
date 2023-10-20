from MetaStudio import app,db,redirect,url_for,LoginManager,render_template
from flask_login import login_required, current_user,logout_user,login_user

from MetaStudio.Models import UserProfile
login_manager = LoginManager()
login_manager.login_view = 'home'
login_manager.init_app(app)

@login_manager.user_loader
def load_user(user_id):
    user = UserProfile.query.filter_by(id=user_id).first()
    if user:
        return user
    return None


@app.route("/")
def home():
    if current_user.is_authenticated == False:
        return render_template("homepage.html",LoggedIn=False)
    else:
        return redirect(url_for("Profile.Dashboard"))

if __name__=="__main__":
    app.run(host="192.168.29.227",port=80)