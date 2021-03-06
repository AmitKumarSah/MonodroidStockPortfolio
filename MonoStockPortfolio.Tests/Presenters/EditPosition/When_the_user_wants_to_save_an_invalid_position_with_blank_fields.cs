﻿using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using MonoStockPortfolio.Activites.EditPositionScreen;
using MonoStockPortfolio.Entities;
using Telerik.JustMock;

namespace MonoStockPortfolio.Tests.Presenters.EditPosition
{
    [Tags("UnitTest")]
    public class When_the_user_wants_to_save_an_invalid_position_with_blank_fields : EditPositionTests
    {
        Establish context = () =>
            {
                _presenter.Initialize(_mockView, 1);

                Mock.Arrange(() => _mockStockService.IsValidTicker(Arg.AnyString)).Returns(false);
            };

        Because of = () =>
            {
                var fakeInputModel = new PositionInputModel { PriceText = "", SharesText = "", TickerText = "" };
                _presenter.Save(fakeInputModel);
            };

        It should_not_try_to_save_the_portfolio = () =>
            Mock.Assert(() => _mockPortfolioRepository.SavePosition(Arg.IsAny<Position>()), Occurs.Never());
        It should_send_the_validation_errors_to_the_view = () =>
            Mock.Assert(() => _mockView.ShowErrorMessages(Arg.IsAny<IList<string>>()), Occurs.Exactly(1));
        It should_send_an_invalid_ticker_error_to_the_view = () =>
            MockPositionMatches(x => x.Any(p => p == "Please enter a ticker"));
        It should_send_an_invalid_shares_number_error_to_the_view = () =>
            MockPositionMatches(x => x.Any(p => p == "Please enter a valid, positive number of shares"));
        It should_send_an_invalid_price_per_share_error_to_the_view = () =>
            MockPositionMatches(x => x.Any(p => p == "Please enter a valid, positive price per share"));
        It should_not_tell_the_view_to_go_back_to_the_main_activity = () =>
            Mock.Assert(() => _mockView.GoBackToPortfolioActivity(), Occurs.Never());
    }
}