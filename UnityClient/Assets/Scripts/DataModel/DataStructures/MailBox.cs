﻿using System.Collections.Generic;

public class MailBox : DataObject {

    public int ID { get; private set; }

    [Newtonsoft.Json.JsonProperty]
    List<Mail> mails = new List<Mail>();

    public MailBox(int id) {
        ID = id;    
    }

    public void AddMail(Mail m) {
        mails.Add(m);
    }    
    
    public void RemoveMail(int mailID) {
        foreach(Mail m in mails) {
            if(m.ID == mailID) {
                mails.Remove(m);
                break;
            }
        }
    }

    public List<Mail> Mails { get { return new List<Mail>(mails); } }

    public override List<EventHolder> Update(DataObject obj) {
        List<EventHolder> result = new List<EventHolder>();
        MailBox m = obj as MailBox;
        if (null == m)
            return result;

        
        if (NeedToUpdate(m)) {
            string data = Newtonsoft.Json.JsonConvert.SerializeObject(m);
            var serializerSettings = new Newtonsoft.Json.JsonSerializerSettings { ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Replace };
            Newtonsoft.Json.JsonConvert.PopulateObject(data, this, serializerSettings);


            result.Add(new MailboxChangeEvent(this, LocalDataManager.instance.OnMailboxChange));
        }
        return result;
    }

    private bool NeedToUpdate(MailBox other) {
        int mailCount = mails.Count;
        if (mails.Count != other.mails.Count)
            return true;

        if (other.Loaded != Loaded)
            return true;

        for(int i = 0; i < mailCount; i++) {
            if (mails[i].ID != other.mails[i].ID)
                return true;
        }

        return false;
    }
}
