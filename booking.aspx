<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="booking.aspx.cs" Inherits="agri.Vendor.booking" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <meta charset="utf-8">
  <meta http-equiv="X-UA-Compatible" content="IE=edge">
  <title>Admin|course Data Tables</title>
  <!-- Tell the browser to be responsive to screen width -->
  <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport">
  <!-- Bootstrap 3.3.7 -->
  <link rel="stylesheet" href=" bower_components/bootstrap/dist/css/bootstrap.min.css">
  <!-- Font Awesome -->
  <link rel="stylesheet" href=" bower_components/font-awesome/css/font-awesome.min.css">
  <!-- Ionicons -->
  <link rel="stylesheet" href=" bower_components/Ionicons/css/ionicons.min.css">
  <!-- DataTables -->
  <link rel="stylesheet" href=" bower_components/datatables.net-bs/css/dataTables.bootstrap.min.css">
  <!-- Theme style -->
  <link rel="stylesheet" href=" dist/css/AdminLTE.min.css">
  <!-- AdminLTE Skins. Choose a skin from the css/skins
       folder instead of downloading all of them to reduce the load. -->
  <link rel="stylesheet" href=" dist/css/skins/_all-skins.min.css">

  <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
  <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
  <!--[if lt IE 9]>
  <script src="https://oss.maxcdn.com/html5shiv/3.7.3/html5shiv.min.js"></script>
  <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
  <![endif]-->

  <!-- Google Font -->
  <link rel="stylesheet"
        href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,600,700,300italic,400italic,600italic">
     <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/toastr.min.css" integrity="sha512-vKMx8UnXk60zUwyUnUPM3HbQo8QfmNx7+ltw8Pm5zLusl1XIfwcxo8DbWCqMGKaWeNxWA8yrx5v3SaVpMvR3CA==" crossorigin="anonymous" />

</head>
<body class="hold-transition skin-blue sidebar-mini">
<div class="wrapper">
  <header class="main-header">
    <!-- Logo -->
    <a href="#" class="logo">
      <!-- mini logo for sidebar mini 50x50 pixels -->
      <span class="logo-mini"><b>A</b>LT</span>
      <!-- logo for regular state and mobile devices -->
      <span class="logo-lg"><b>AGRI</b>XPERT</span>
    </a>
    <!-- Header Navbar: style can be found in header.less -->
    <nav class="navbar navbar-static-top">
      <!-- Sidebar toggle button-->
      <a href="#" class="sidebar-toggle" data-toggle="push-menu" role="button">
        <span class="sr-only">Toggle navigation</span>
      </a>

      <div class="navbar-custom-menu">
        <ul class="nav navbar-nav">
          <!-- Messages: style can be found in dropdown.less-->
      
          <!-- Notifications: style can be found in dropdown.less -->

          <!-- Tasks: style can be found in dropdown.less -->
     
          <!-- User Account: style can be found in dropdown.less -->
          <li class="dropdown user user-menu">
            <a href="#" class="dropdown-toggle" data-toggle="dropdown">
             
              <span class="hidden-xs">Admin</span>
            </a>
            <ul class="dropdown-menu">
              <!-- User image -->
           
              <!-- Menu Body -->
          
              <!-- Menu Footer-->
              <li class="user-footer">
              
                <div class="pull-right">
                  <a href="../index.aspx" class="btn btn-default btn-flat">Sign out</a>
                </div>
              </li>
            </ul>
          </li>
  
    
        </ul>
      </div>
    </nav>
  </header>

  <aside class="main-sidebar">

    <section class="sidebar">

      <ul class="sidebar-menu" data-widget="tree">
        <li class="header">MAIN NAVIGATION</li>
		   <li class="active"><a href="index.aspx"><i class="fa fa-dashboard"></i> <span>Dashboard</span></a></li>
        <li class="active treeview">
		   
        </li>

        <li class="header">User Option</li>
       
         <li class="treeview">
          <a href="#">
            <i class="fa fa-files-o"></i>
            <span>Product</span>
            <span class="pull-right-container">
              <span class="label label-primary pull-right"></span>
            </span>
          </a>
           <ul class="treeview-menu">
     
            <li><a href="AddProduct.aspx"><i class="fa fa-circle-o"></i> Add Product</a></li>
            <li><a href="ManageProduct.aspx"><i class="fa fa-circle-o"></i>Manage Product</a></li>
        
         
          </ul>
        </li>

       <li><a href="booking.aspx"><i class="fa fa-circle-o text-yellow"></i> <span>Booking</span></a></li>

   
        <li><a href="../logout.aspx"><i class="fa fa-sign-out text-aqua"></i> <span>Logout</span></a></li>
      </ul>
    </section>
    <!-- /.sidebar -->
  </aside>

  <!-- Content Wrapper. Contains page content -->
  <div class="content-wrapper">
    <!-- Content Header (Page header) -->
    <section class="content-header">
      <h1>
        
        <small></small>
        <small></small>

      </h1>

      <ol class="breadcrumb">
        <li><a href="#"><i class="fa fa-dashboard"></i> Home</a></li>
        <li><a href="#">Tables</a></li>
        <li class="active">Course table</li>
      </ol>

    </section>

    <!-- Main content -->
      <form id="form1" runat="server">  
    <section class="content">
      <div class="row">
        <div class="col-xs-12">
        
          <!-- /.box -->

          <div class="box">
              <h3 class="box-title">Booking Data</h3>
         
            <!-- /.box-header -->
            <div class="box-body">
 <table id="example1" class="table table-bordered table-striped">
    <thead>
        <tr>
            <th>Order & Product Summary</th>
        </tr>
    </thead>
    <tbody>
        <asp:Repeater ID="RepeatInformation" runat="server">
            <ItemTemplate>
                <tr>
                    <td>
                        <strong>Order ID:</strong> <%# Eval("OrderID") %><br />
                        <strong>User ID:</strong> <%# Eval("UserID") %><br />
                        <strong>Order Date:</strong> <%# Eval("OrderDate") %><br />
                        <strong>Total Amount:</strong> ₹<%# Eval("TotalAmount") %><br />
                        <strong>Name:</strong> <%# Eval("FirstName") %> <%# Eval("LastName") %><br />
                        <strong>Company:</strong> <%# Eval("CompanyName") %><br />
                        <strong>Address:</strong> <%# Eval("AddressLine1") %>, <%# Eval("AddressLine2") %>, <%# Eval("City") %>, <%# Eval("State") %>, <%# Eval("Country") %> - <%# Eval("ZipCode") %><br />
                        <strong>Phone:</strong> <%# Eval("Phone") %><br />
                        <strong>Email:</strong> <%# Eval("Email") %><br />
                        <strong>Notes:</strong> <%# Eval("OrderNotes") %><br />
                        <hr />
                        <strong>Product:</strong> <%# Eval("ProductName") %> (<%# Eval("pid") %>)<br />
                        <strong>Quantity:</strong> <%# Eval("Quantity") %><br />
                        <strong>Price:</strong> ₹<%# Eval("Price") %><br />
                        <strong>Type:</strong> <%# Eval("ptype") %><br />
                        <strong>Description:</strong> <%# Eval("descp") %><br />
                        <strong>Amount:</strong> ₹<%# Eval("amt") %><br />
                        <strong>Category:</strong> <%# Eval("Category") %><br />
                        <strong>Status:</strong> <%# Eval("pstatus") %><br />
                        <strong>Business ID:</strong> <%# Eval("BusinessID") %><br />
                        <strong>Created On:</strong> <%# Convert.ToDateTime(Eval("createdon")).ToString("dd/MM/yyyy") %><br />
                        <strong>Available Quantity:</strong> <%# Eval("quantity") %><br />
                        <strong>Images:</strong><br />
                        <img src='../<%# Eval("img1") %>' style="width:50px;" />
                        <img src='../<%# Eval("img2") %>' style="width:50px;" />
                        <img src='../<%# Eval("img3") %>' style="width:50px;" />
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </tbody>
</table>

            </div>
            <!-- /.box-body -->
          </div>
          <!-- /.box -->
        </div>
        <!-- /.col -->
      </div>
      <!-- /.row -->
    </section>
          </form>
    <!-- /.content -->
  </div>
  <!-- /.content-wrapper -->
  <footer class="main-footer">
    <div class="pull-right hidden-xs">
      <b>Version</b> 2.4.0
    </div>
    <strong>Copyright &copy; 2021-2022 <a target="_blank" href="">admin@2022</a>.</strong> All rights
    reserved.
  </footer>

  <!-- Control Sidebar -->
 
  <!-- /.control-sidebar -->
  <!-- Add the sidebar's background. This div must be placed
       immediately after the control sidebar -->
  <div class="control-sidebar-bg"></div>
</div>
<!-- ./wrapper -->

<!-- jQuery 3 -->
<script src=" bower_components/jquery/dist/jquery.min.js"></script>
<!-- Bootstrap 3.3.7 -->
<script src=" bower_components/bootstrap/dist/js/bootstrap.min.js"></script>
<!-- DataTables -->
<script src=" bower_components/datatables.net/js/jquery.dataTables.min.js"></script>
<script src=" bower_components/datatables.net-bs/js/dataTables.bootstrap.min.js"></script>
<!-- SlimScroll -->
<script src=" bower_components/jquery-slimscroll/jquery.slimscroll.min.js"></script>
<!-- FastClick -->
<script src=" bower_components/fastclick/lib/fastclick.js"></script>
<!-- AdminLTE App -->
<script src=" dist/js/adminlte.min.js"></script>
<!-- AdminLTE for demo purposes -->
<script src=" dist/js/demo.js"></script>


         <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/js/toastr.min.js"></script>
    <script src="https://cdn.yz.events/js/jquery.rwdImageMaps.js"></script>
    <script src="https://cdn.yz.events/js/custom.js"></script>
    

<%--    <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>--%>
<!-- page script -->
<script>
    $(function () {
        $('#example1').DataTable()
        $('#example2').DataTable({
            'paging': true,
            'lengthChange': false,
            'searching': false,
            'ordering': true,
            'info': true,
            'autoWidth': false
        })
    })
</script>

    <script>
    
               function ClaimDelete(id) {
                   
                       //alert(id);
                       $.ajax({
                           type: 'POST',
                           contentType: "application/json; charset=utf-8",
                           dataType: "json",
                           data: JSON.stringify({ ID: id }),
                           url: "ManageProduct.aspx/deleteClaim",
                           success: function (data) {
                               if (data.d == 1) {

                                   alert("Data deleted");
                                   //toastr.success("Image Requested.");
                                   window.location.reload();

                               } else {
                                   toastr.error("Error in deleting record.");
                               }

                           }
                     
                   });
               }
    </script>
</body>
</html>
