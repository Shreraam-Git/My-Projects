<%@ Page Title="Rules & Regulations" Language="C#" MasterPageFile="~/Formula1/F1MasterPage.master" AutoEventWireup="true" CodeFile="RulesandRegulation.aspx.cs" Inherits="Formula1_RulesandRegulation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">    
    <div class="content-wrapper container" style="opacity: 0.9;">        
        <div class="page-content">            
            <section class="row">
                <div class="col-lg-12 col-md-12">
                    <div class="card">                        
                        <div class="card-content">
                            <div class="card-body">                                
                                <ul class="list-group">
                                    <li style="text-align:center; font-size:30px;" class="list-group-item active">Rules & Regulations</li>
                                    <asp:Label runat="server" ID="RulesandRegulation"></asp:Label>                                    
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </div>        
    </div>
</asp:Content>

