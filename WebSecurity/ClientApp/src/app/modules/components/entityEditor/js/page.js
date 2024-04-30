    $(function() {
      var ds = {
        name: "Lao Lao",
        title: "general manager",
        children: [
          { name: "Bo Miao", title: "department manager" },
          {
            name: "Su Miao",
          title: "department manager",
          children: [
            { name: "Tie Hua", title: "senior engineer" },
            {
              name: "Hei Hei",
              title: "senior engineer",
              children: [
                { name: "Pang Pang", title: "engineer" },
                {
                  name: "Xiang Xiang",
                  title: "UE engineer",
                  children: [
                    { name: "Dan Dan", title: "engineer" },
                    {
                      name: "Er Dan",
                      title: "engineer",
                      children: [
                        { name: "Xuan Xuan", title: "intern" },
                        { name: "Er Xuan", title: "intern" }
                      ]
                    }
                  ]
                }
              ]
            }
          ]
        },
        { name: "Hong Miao", title: "department manager" },
        {
          name: "Chun Miao",
          title: "department manager",
          children: [
            { name: "Bing Qin", title: "senior engineer" },
            {
              name: "Yue Yue",
              title: "senior engineer",
              children: [
                { name: "Er Yue", title: "engineer" },
                { name: "San Yue", title: "UE engineer" }
              ]
            }
          ]
        }
      ]
    };

    //var form = $("#form-container").carouselForm();

    var getId = function() {
      return (new Date().getTime()) * 1000 + Math.floor(Math.random() * 1001);
    };
  });
