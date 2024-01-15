namespace DecoratorDesignPatternTests;
public class Urls
{
    public const string BASE_URL = "https://ecommerce-playground.lambdatest.io/";
    public const string SEARCH_SHOP_PRODUCTS_PAGE = BASE_URL + "index.php?route=product%2Fsearch&search=&limit=100";
    public const string HOME_PAGE = BASE_URL + "index.php?route=common/home";
    public const string SEARCH_PRODUCTS_PAGE = BASE_URL + "index.php?route=product%2Fsearch&search=";
    public const string PRODUCT_PAGE = BASE_URL + "index.php?route=product/product";
    public const string CART_PAGE = BASE_URL + "index.php?route=checkout/cart";
    public const string CHECKOUT_PAGE = BASE_URL + "index.php?route=checkout/checkout";
    public const string LOGIN_PAGE = BASE_URL + "index.php?route=account/login";
    public const string REGISTER_PAGE = BASE_URL + "index.php?route=account/register";
    public const string BLOG_HOME_PAGE = BASE_URL + "index.php?route=extension/maza/blog/home";
    public const string BLOG_ARTICLE_PAGE = BASE_URL + "index.php?route=extension/maza/blog/article&article_id={0}";
    public const string WISHLIST_PAGE = BASE_URL + "index.php?route=account/wishlist";
    public const string COMPARISON_PAGE = BASE_URL + "index.php?route=product/compare";
    public const string SUCCESSFUL_REGISTRATION_PAGE = BASE_URL + "index.php?route=account/success";
    public const string FORGOTTEN_PASSWORD_PAGE = BASE_URL + "index.php?route=account/forgotten";
    public const string ORDER_HISTORY_PAGE = BASE_URL + "index.php?route=account/order";
    public const string ORDER_PAGE = BASE_URL + "index.php?route=account/order/info";
    public const string ACCOUNT_PAGE = BASE_URL + "index.php?route=account/account";
    public const string ADDRESS_BOOK_PAGE = BASE_URL + "index.php?route=account/address";
    public const string NEW_ADDRESS_PAGE = BASE_URL + "index.php?route=account/address/add";
    public const string EDIT_ADDRESS_PAGE = BASE_URL + "index.php?route=account/address/edit";
    public const string SUCCESSFUL_ORDER_PAGE = BASE_URL + "index.php?route=checkout/success";
}
